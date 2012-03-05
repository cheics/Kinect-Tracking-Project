function truAcc=getTrueAccuracy(obj, testPoints, truthClasses)
	predictedClasses=obj.classifyLots(testPoints);
	function ul=makeUniqLabels(critCompClasses)
		ul=critCompClasses(:,1)*1+critCompClasses(:,2)*10+critCompClasses(:,3)*100;
	end
	truAcc_cm=confusionmat(makeUniqLabels(truthClasses),makeUniqLabels(predictedClasses));
	truAcc=sum(sum(diag(truAcc_cm))) / sum(sum(truAcc_cm));
end