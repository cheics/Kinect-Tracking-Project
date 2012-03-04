classdef Classifier_BASE < handle

	properties (Access = public, Hidden = false)
		filenameOut
		featureSize=-1;
		classifierType='undefinedClassifier';
		classifierName='noName';
		baseDir='SavedClassifiers';
	end
	
	methods
		function obj = Classifier_BASE(classifierName)
				obj.classifierName=classifierName;
		end
			
		function [dataSet2,dataSet1,dataSet0]=splitTrainingData(obj, tData, cData)
			score2_ind= cData == 2;
			score1_ind= cData == 1;
			score0_ind= cData == 0;
			dataSet2=tData(score2_ind,:);
			dataSet1=tData(score1_ind,:);
			dataSet0=tData(score0_ind,:);
		end
	
		function featureSizeCheck(obj, testPoint)
			if size(testPoint, 2) ~= obj.featureSize
				err = MException('ResultChk:BadInput', ...
					'Classifier operates on feature vector of size %i, not feature vector of size %i',...
					obj.featureSize, ...
					size(testPoint, 2) ...
				);
				throw(err)
			end
		end
			
	end
	
	methods(Abstract)
		outClass=classify(obj, testPoint)	
		trainClassifier(obj, tData, cData, featureSize)
	end
	

	
end