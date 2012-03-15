classdef MakeClassifier < handle

	properties (Access = public, Hidden = false)
		theClassifier
		theExcercise
	end
	
	properties (Access = public, Hidden = true)
		availibleClassifiers={'GED','MAP', 'BAYES', 'SVM'};
		availibleExercise={'squats','armRaise', 'legRaise', 'legExt'};
	end
	
	methods (Access = public)
		function obj = MakeClassifier(typeClassifier, loadFromFile, varargin)
	
		if numel(varargin)>0
			obj.theExcercise=varargin{1};
		else
			obj.theExcercise='squats';
		end
		
			%% Case statements in order to choose the critComp
			if strcmp(obj.theExcercise, 'squats')
				cc1='squatDepth'; cc2='straightBack'; cc2='squatBal';
            elseif strcmp(obj.theExcercise, 'armRaise')
                cc1='straightArm'; cc2='handsInFront'; cc2='handsHigh';
			elseif strcmp(obj.theExcercise, 'legRaise')
                cc1='kneeAngle'; cc2='spineStability'; cc2='hipAbduction';
			elseif strcmp(obj.theExcercise, 'legExt')
                cc1='hipsLevel'; cc2='hipAbductionAngle'; cc2='spineStability';
            else
				err = MException('ResultChk:BadInput', ...
					'Exercise type %s is not valid, must be one of {%s}',...
					obj.theExcercise, ...
					strtrim(sprintf('%s ', obj.availibleExercise{:})) ...
				);
				throw(err)
			end
			
		%% Case statements in order to choose the classifier
		name_cc1=sprintf('CC1-%s', cc1);
		name_cc2=sprintf('CC2-%s', cc1);
		name_cc3=sprintf('CC3-%s', cc1);
			if strcmp(typeClassifier, 'GED')
				obj.theClassifier.critComp1=Classifier_GED(name_cc1);
				obj.theClassifier.critComp2=Classifier_GED(name_cc2);
				obj.theClassifier.critComp3=Classifier_GED(name_cc3);
            elseif strcmp(typeClassifier, 'MAP')
                obj.theClassifier.critComp1=Classifier_MAP(name_cc1);
				obj.theClassifier.critComp2=Classifier_MAP(name_cc2);
				obj.theClassifier.critComp3=Classifier_MAP(name_cc3);
			elseif strcmp(typeClassifier, 'BAYES')
                obj.theClassifier.critComp1=Classifier_BAYES(name_cc1);
				obj.theClassifier.critComp2=Classifier_BAYES(name_cc2);
				obj.theClassifier.critComp3=Classifier_BAYES(name_cc3);
			elseif strcmp(typeClassifier, 'SVM')
                obj.theClassifier.critComp1=Classifier_SVM(name_cc1);
				obj.theClassifier.critComp2=Classifier_SVM(name_cc2);
				obj.theClassifier.critComp3=Classifier_SVM(name_cc3);
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
