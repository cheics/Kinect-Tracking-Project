function [accuracy,truAcc]=getAccuracy(obj, testPoints, truthClasses)
	[cm1, cm2, cm3]=obj.makeConfusionMats(testPoints, truthClasses);
	acc_cm1=sum(sum(diag(cm1))) / sum(sum(cm1));
	acc_cm2=sum(sum(diag(cm2))) / sum(sum(cm2));
	acc_cm3=sum(sum(diag(cm3))) / sum(sum(cm3));

	accuracy=mean([acc_cm1,acc_cm2,acc_cm3]);
	truAcc=obj.getTrueAccuracy(testPoints, truthClasses);
end