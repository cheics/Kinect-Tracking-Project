classdef MakeClassifier < handle

	properties (Access = public, Hidden = false)
		theClassifier
		availibleClassifiers={'GED'};
	end
	
	methods
		function obj = MakeClassifier(typeClassifier, loadFromFile)
			obj.theClassifier=struct();
			
		%% Case statements in order to choose the classifier
			if strcmp(typeClassifier, 'GED')
				obj.theClassifier.critComp1=Classifier_GED('CC1-squatDepth');
				obj.theClassifier.critComp2=Classifier_GED('CC2-straightBack');
				obj.theClassifier.critComp3=Classifier_GED('CC3-squatBal');
			else
				err = MException('ResultChk:BadInput', ...
					'Classifier type %s is not valid, must be one of {%s}',...
					typeClassifier, ...
					strtrim(sprintf('%s ', availibleClassifiers{:})) ...
				);
				throw(err)
			end
			
		%% Can also load existing
			if loadFromFile == true
				obj.loadClassifier()
			end
				
		end
		
		function trainClassifier(obj, trainingData, classData)
			% first remove all zero features
			trainingData=trainingData(:,any(trainingData));
			featureSize=size(trainingData,2);
			obj.theClassifier.critComp1.createClassifier(trainingData,classData(:,1),featureSize);
			obj.theClassifier.critComp2.createClassifier(trainingData,classData(:,2),featureSize);
			obj.theClassifier.critComp3.createClassifier(trainingData,classData(:,3),featureSize);		
		end
		
		function saveClassifier(obj)
			obj.theClassifier.critComp1.saveClassifier();
			obj.theClassifier.critComp2.saveClassifier();
			obj.theClassifier.critComp3.saveClassifier();
		end
		
		function loadClassifier(obj)
			obj.theClassifier.critComp1.loadClassifier();
			obj.theClassifier.critComp2.loadClassifier();
			obj.theClassifier.critComp3.loadClassifier();
		end

		function [c1,c2,c3] = classify(obj, testPoint)
			c1=obj.theClassifier.critComp1.classify(testPoint);
			c2=obj.theClassifier.critComp2.classify(testPoint);
			c3=obj.theClassifier.critComp3.classify(testPoint);
		end
		
	end
	
end
