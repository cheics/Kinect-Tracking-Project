function trainClassifiers(obj, trainingData, classData)
	% first remove all zero features
	tdTrim=trainingData(:,any(trainingData));
	featureSize=size(tdTrim,2);
	obj.theClassifier.critComp1.trainClassifier(tdTrim,classData(:,1),featureSize);
	obj.theClassifier.critComp2.trainClassifier(tdTrim,classData(:,2),featureSize);
	obj.theClassifier.critComp3.trainClassifier(tdTrim,classData(:,3),featureSize);		
end