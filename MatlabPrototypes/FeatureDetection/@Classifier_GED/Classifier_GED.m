classdef Classifier_GED < handle

	properties (Access = public, Hidden = false)
		classMeans
		classCov
		classifierName
	end
	
	methods
		function obj = Classifier_GED(classifierName)
				obj.classMeans=struct();
				obj.classCov=struct();
				obj.classifierName=classifierName;
		end
		
		function createClassifier(obj, tData, cData)		
			%% Using Logical indexing for speed
			score2_ind= cData == 2;
			score1_ind= cData == 1;
			score0_ind= cData == 0;
					
			dataSet2=tData(score2_ind,:);
			dataSet1=tData(score1_ind,:);
			dataSet0=tData(score0_ind,:);
			
			obj.classMeans.score2=mean(dataSet2);
			obj.classMeans.score1=mean(dataSet1);
			obj.classMeans.score0=mean(dataSet0);
			
			obj.classCov.score2=cov(dataSet2);
			obj.classCov.score1=cov(dataSet1);
			obj.classCov.score0=cov(dataSet0);			
		end
		
		function outClass=classify(obj, testPoint)
		    d2 = obj.getGedDist(obj.classMeans.score2, obj.classCov.score2, testPoint);
			d1 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			d0 = obj.getGedDist(obj.classMeans.score1, obj.classCov.score1, testPoint);
			[minDist,outClass]=min([d0,d1,d2]);
			outClass=outClass-1; %zero index scores, vs matlab 1 index
		end
		
		function gedDist = getGedDist(obj, mean, covar, testpoint)
			gedDist=(mean-testpoint) *  (inv(covar))  * transpose(mean-testpoint);
		end
		
		function saveClassifier(obj)
		end
		
		function loadClassifier(obj)
		end

	end
	

	
end
