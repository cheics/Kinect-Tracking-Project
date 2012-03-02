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
				obj.theClassifier.critComp1=Classifier_GED('CC1_squatDepth');
				obj.theClassifier.critComp2=Classifier_GED('CC2_straightBack');
				obj.theClassifier.critComp3=Classifier_GED('CC3_squatBal');
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
				obj.theClassifier.critComp1.loadClassifer();
				obj.theClassifier.critComp2.loadClassifer();
				obj.theClassifier.critComp3.loadClassifer();
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
			obj.theClassifier.critComp1.saveClassifer();
			obj.theClassifier.critComp2.saveClassifer();
			obj.theClassifier.critComp3.saveClassifer();
		end
		
		function loadClassifier(obj)
			obj.theClassifier.critComp1.loadClassifer();
			obj.theClassifier.critComp2.loadClassifer();
			obj.theClassifier.critComp3.loadClassifer();
		end

		function [c1,c2,c3] = classify(obj, testPoint)
			c1=obj.theClassifier.critComp1.classify(testPoint);
			c2=obj.theClassifier.critComp2.classify(testPoint);
			c3=obj.theClassifier.critComp3.classify(testPoint);
		end
		
	end
	
end
