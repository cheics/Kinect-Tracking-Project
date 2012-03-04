classdef Classifier_MAP < Classifier_GED

	methods
		function obj = Classifier_MAP(classifierName)
			obj@Classifier_GED(classifierName);
			obj.classifierType='MAP';
		end
	
		function outClass=classify(obj, testPoint)
			obj.featureSizeCheck(testPoint); % Ensure sizes

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
		

	end
	

	
end