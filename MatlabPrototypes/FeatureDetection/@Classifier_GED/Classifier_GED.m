classdef Classifier_GED < Classifier_BASE

	properties (Access = public, Hidden = false)
		classMeans
		classCov
	end
	
	methods
		function obj = Classifier_GED(classifierName)
				obj@Classifier_BASE(classifierName);
				obj.classifierType='GED';
				%% Params that define the classifier
				obj.classMeans=struct();
				obj.classCov=struct();
		end
		
		function trainClassifier(obj, tData, cData, featureSize)
			obj.featureSize=featureSize;
			%% Using Logical indexing for speed
			[ds2,ds1,ds0]=obj.splitTrainingData(tData, cData);
			
			obj.classMeans.score2=mean(ds2);
			obj.classMeans.score1=mean(ds1);
			obj.classMeans.score0=mean(ds0);
			
			obj.classCov.score2=cov(ds2);
			obj.classCov.score1=cov(ds1);
			obj.classCov.score0=cov(ds0);			
		end
		
		function outClass=classify(obj, testPoint)
			obj.featureSizeCheck(testPoint); % Ensure sizes

		    d2 = obj.getGedDist(obj.classMeans.score2, obj.classCov.score2, testPoint);
			d1 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			d0 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			[minDist,outClass]=min([d0,d1,d2]);
			outClass=outClass-1; %zero index scores, vs matlab 1 index
		end
		
		function gedDist = getGedDist(obj, mean, covar, testpoint)
			gedDist=(mean-testpoint) *  (inv(covar))  * transpose(mean-testpoint);
		end
		
	end
	

	
end
