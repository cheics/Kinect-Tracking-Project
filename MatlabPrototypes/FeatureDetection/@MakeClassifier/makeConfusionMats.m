function [cm1, cm2, cm3]=makeConfusionMats(obj, testPoints, truthClasses)
	predictedClasses=obj.classifyLots(testPoints);
	cm1=confusionmat(truthClasses(:,1),predictedClasses(:,1));
	cm2=confusionmat(truthClasses(:,2),predictedClasses(:,2));
	cm3=confusionmat(truthClasses(:,3),predictedClasses(:,3));
end