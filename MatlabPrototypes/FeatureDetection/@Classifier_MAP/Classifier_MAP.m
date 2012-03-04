classdef Classifier_MAP < handle

	properties (Access = public, Hidden = false)
		classMeans
		classCov
		filenameOut
		featureSize
	end
	
	methods
		function obj = Classifier_MAP(classifierName)
				obj.classMeans=struct();
				obj.classCov=struct();
				obj.featureSize=0; %% Until trained
				obj.filenameOut=fullfile('SavedClassifiers', ...
					sprintf('%s_%s.mat', 'MAP', classifierName)...
				);
		end
		
		function createClassifier(obj, tData, cData, featureSize)
			obj.featureSize=featureSize;
			%% Using Logical indexing for speed
			score2_ind= cData == 2;
			score1_ind= cData == 1;
			score0_ind= cData == 0;
					
			dataSet2=tData(score2_ind,:);
			dataSet1=tData(score1_ind,:);
			dataSet0=tData(score0_ind,:);
			
			obj.classMeans.score2=mean(dataSet2);
			obj.classMeans.score1=mean(dataSet1);
			obj.classMeans.score0=mean(dataSet0);
			
			obj.classCov.score2=cov(dataSet2);
			obj.classCov.score1=cov(dataSet1);
			obj.classCov.score0=cov(dataSet0);			
		end
		
		function outClass=classify(obj, testPoint)
			if size(testPoint, 2) ~= obj.featureSize
				err = MException('ResultChk:BadInput', ...
					'Classifier operates on feature vector of size %i, not feature vector of size %i',...
					obj.featureSize, ...
					size(testPoint, 2) ...
				);
				throw(err)
			end
		    comp1_0 = obj.compareMAP(obj.classMeans.score0, obj.classCov.score0,...
                obj.classMeans.score1, obj.classCov.score1, testPoint);
			comp2_0 = obj.compareMAP(obj.classMeans.score0, obj.classCov.score0,...
                obj.classMeans.score2, obj.classCov.score2, testPoint);
			comp2_1 = obj.compareMAP(obj.classMeans.score1, obj.classCov.score1,...
                obj.classMeans.score2, obj.classCov.score2, testPoint);
            
			outClass=obj.getMAPclass(comp1_0,comp2_0,comp2_1);
		end
		
		function mapCompare = compareMAP(obj,mean1,covar1,mean2,covar2,testpoint)
			classDist=(mean1-testpoint) *  (inv(covar1))  * transpose(mean1-testpoint) ...
                    - (mean2-testpoint) *  (inv(covar2))  * transpose(mean2-testpoint) ...
                    - log(det(covar2)/det(covar1)) ;
			mapCompare=sign(classDist);
		end
		
		function class = getMAPclass(obj, comp1_0, comp2_0, comp2_1)
			if comp1_0 == -1 && comp2_0 == -1
				class=0;
			elseif comp1_0 == 1 && comp2_1 == -1
				class=1;
			elseif comp2_0 == 1 && comp2_1 == 1
				class=2;
			else
				class=-1;
			end
		end
		
		function saveClassifier(obj)
			MAP_classifier=struct();
			MAP_classifier.featureSize=obj.featureSize;
			MAP_classifier.classMeans=obj.classMeans;
			MAP_classifier.classCov=obj.classCov;
			save(obj.filenameOut, 'MAP_classifier');
		end
		
		function loadClassifier(obj)
			load(obj.filenameOut);
			obj.classMeans=MAP_classifier.classMeans;
			obj.classCov=MAP_classifier.classCov;
			obj.featureSize=MAP_classifier.featureSize;
		end

	end
	

	
end