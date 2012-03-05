classdef Classifier_BAYES < Classifier_BASE

	properties (Access = public, Hidden = false)
		bayesClass
	end
	
	methods
		function obj = Classifier_BAYES(classifierName)
				obj@Classifier_BASE(classifierName);
				obj.classifierType='BAYES';
				%% Params that define the classifier
		end
		
		function trainClassifier(obj, tData, cData, featureSize)
			obj.featureSize=featureSize;
			obj.bayesClass = NaiveBayes.fit(...
				tData, cData, ...
				'Distribution', 'kernel', ...
				'Prior', 'uniform' ...
			);
 		
		end
		
		function outClass=classify(obj, testPoint)
			obj.featureSizeCheck(testPoint); % Ensure sizes
			outClass=obj.bayesClass.predict(testPoint);
		end
		
	end
	

	
end
