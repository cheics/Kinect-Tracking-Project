classdef Classifier_SVM < Classifier_BASE

	properties (Access = public, Hidden = false)
		classModel
	end
	
	methods
		function obj = Classifier_SVM(classifierName)
				obj@Classifier_BASE(classifierName);
				obj.classifierType='SVM';
				%% Params that define the classifier
				obj.classModel=struct();
		end
		
		function obj = trainClassifier(obj, tData, cData, featureSize)
            
			obj.featureSize=featureSize;			
            obj.classModel = svmtrain(cData, tData);
					
		end
		
		function outClass= classify(obj,  testPoint)
			obj.featureSizeCheck(testPoint); % Ensure sizes
 
            [outClass, accuracy, probEstimates] = svmpredict(randi(1,1), testPoint, obj.classModel);
		    
        end
        
        %function [outClass, accuracy, probEstimates] =classifyAcc(obj,  testPoint, label)
		%	obj.featureSizeCheck(testPoint); % Ensure sizes
 
        %    [outClass, accuracy, probEstimates] = svmpredict(label, testPoint, obj.classModel);
		    
		%	outClass=outClass-1; %zero index scores, vs matlab 1 index
		%end

	end
	

	
end