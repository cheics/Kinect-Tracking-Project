classdef Classifier_GED < handle

	properties (Access = public, Hidden = false)
		classMeans
		classCov
		filenameOut
		featureSize
	end
	
	methods
		function obj = Classifier_GED(classifierName)
				obj.classMeans=struct();
				obj.classCov=struct();
				obj.featureSize=0; %% Until trained
				obj.filenameOut=fullfile('SavedClassifiers', ...
					sprintf('%s_%s.mat', 'GED', classifierName)...
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
		    d2 = obj.getGedDist(obj.classMeans.score2, obj.classCov.score2, testPoint);
			d1 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			d0 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			[minDist,outClass]=min([d0,d1,d2]);
			outClass=outClass-1; %zero index scores, vs matlab 1 index
		end
		
		function gedDist = getGedDist(obj, mean, covar, testpoint)
			gedDist=(mean-testpoint) *  (inv(covar))  * transpose(mean-testpoint);
		end
		
		function saveClassifier(obj)
			GED_classifier=struct();
			GED_classifier.featureSize=obj.featureSize;
			GED_classifier.classMeans=obj.classMeans;
			GED_classifier.classCov=obj.classCov;
			save(obj.filenameOut, 'GED_classifier');
		end
		
		function loadClassifier(obj)
			load(obj.filenameOut);
			obj.classMeans=GED_classifier.classMeans;
			obj.classCov=GED_classifier.classCov;
			obj.featureSize=GED_classifier.featureSize;
		end

	end
	

	
end
