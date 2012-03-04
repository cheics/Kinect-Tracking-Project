classdef MakeClassifier < handle

	properties (Access = public, Hidden = false)
		theClassifier
	end
	
	properties (Access = public, Hidden = true)
		availibleClassifiers={'GED','MAP'};
	end
	
	methods
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
		
		function trainClassifiers(obj, trainingData, classData)
			% first remove all zero features
			tdTrim=trainingData(:,any(trainingData));
			featureSize=size(tdTrim,2);
			obj.theClassifier.critComp1.trainClassifier(tdTrim,classData(:,1),featureSize);
			obj.theClassifier.critComp2.trainClassifier(tdTrim,classData(:,2),featureSize);
			obj.theClassifier.critComp3.trainClassifier(tdTrim,classData(:,3),featureSize);		
		end
		
		function filenameOut=getClassifierName(obj, baseDir, cType, cName)
			filenameOut=fullfile(baseDir, sprintf('%s_%s.mat', cType, cName));
		end
		
		function saveClassifier(obj)
			c1=obj.theClassifier.critComp1;
			c2=obj.theClassifier.critComp2;
			c3=obj.theClassifier.critComp3;
			c1_n=obj.getClassifierName(c1.baseDir, c1.classifierType, c1.classifierName);
			c2_n=obj.getClassifierName(c2.baseDir, c2.classifierType, c2.classifierName);
			c3_n=obj.getClassifierName(c3.baseDir, c3.classifierType, c3.classifierName);
			save(c1_n,  'c1');
			save(c2_n,  'c2');
			save(c3_n,  'c3');
		end
		
		function loadClassifier(obj)
			c1=obj.theClassifier.critComp1;
			c2=obj.theClassifier.critComp2;
			c3=obj.theClassifier.critComp3;
			c1_n=obj.getClassifierName(c1.baseDir, c1.classifierType, c1.classifierName);
			c2_n=obj.getClassifierName(c2.baseDir, c2.classifierType, c2.classifierName);
			c3_n=obj.getClassifierName(c3.baseDir, c3.classifierType, c3.classifierName);
			load(c1_n,  'c1');
			load(c2_n,  'c2');
			load(c3_n,  'c3');
			obj.theClassifier.critComp1=c1;
			obj.theClassifier.critComp2=c2;
			obj.theClassifier.critComp3=c3;
		end

		function classification = classify(obj, testPoint)
			c1=obj.theClassifier.critComp1.classify(testPoint);
			c2=obj.theClassifier.critComp2.classify(testPoint);
			c3=obj.theClassifier.critComp3.classify(testPoint);
			classification=[c1,c2,c3];
		end
		
	end
	
end
