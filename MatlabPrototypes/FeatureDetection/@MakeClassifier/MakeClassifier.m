classdef MakeClassifier < handle

	properties (Access = public, Hidden = false)
		theClassifier
	end
	
	properties (Access = public, Hidden = true)
		availibleClassifiers={'GED','MAP'};
	end
	
	methods (Access = public)
		function obj = MakeClassifier(typeClassifier, loadFromFile)
			obj.theClassifier=struct();
			
		%% Case statements in order to choose the classifier
			if strcmp(typeClassifier, 'GED')
				obj.theClassifier.critComp1=Classifier_GED('CC1-squatDepth');
				obj.theClassifier.critComp2=Classifier_GED('CC2-straightBack');
				obj.theClassifier.critComp3=Classifier_GED('CC3-squatBal');
            elseif strcmp(typeClassifier, 'MAP')
                obj.theClassifier.critComp1=Classifier_MAP('CC1-squatDepth');
				obj.theClassifier.critComp2=Classifier_MAP('CC2-straightBack');
				obj.theClassifier.critComp3=Classifier_MAP('CC3-squatBal');
			elseif strcmp(typeClassifier, 'BAYES')
                obj.theClassifier.critComp1=Classifier_BAYES('CC1-squatDepth');
				obj.theClassifier.critComp2=Classifier_BAYES('CC2-straightBack');
				obj.theClassifier.critComp3=Classifier_BAYES('CC3-squatBal');
            else
				err = MException('ResultChk:BadInput', ...
					'Classifier type %s is not valid, must be one of {%s}',...
					typeClassifier, ...
					strtrim(sprintf('%s ', obj.availibleClassifiers{:})) ...
				);
				throw(err)
			end
			
		%% Can also load existing
			if loadFromFile == true
				obj.loadClassifier()
			end
				
		end
		
		function filenameOut=getClassifierName(obj, baseDir, cType, cName)
			filenameOut=fullfile(baseDir, sprintf('%s_%s.mat', cType, cName));
		end
	end
	
	methods (Access = public)
		trainClassifiers(obj, trainingData, classData)	
		saveClassifier(obj)	
		loadClassifier(obj)
		classification = classify(obj, testPoint)
		classifications = classifyLots(obj, testPoints)
	end

	methods (Access = public)
			[cm1, cm2, cm3]=makeConfusionMats(obj, testPoints, truthClasses)
			[accuracy,truAcc]=getAccuracy(obj, testPoints, truthClasses)
			truAcc=getTrueAccuracy(obj, testPoints, truthClasses)
	end
	
end
