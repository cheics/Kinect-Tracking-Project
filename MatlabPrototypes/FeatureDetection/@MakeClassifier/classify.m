function classification = classify(obj, testPoint)
	c1=obj.theClassifier.critComp1.classify(testPoint);
	c2=obj.theClassifier.critComp2.classify(testPoint);
	c3=obj.theClassifier.critComp3.classify(testPoint);
	classification=[c1,c2,c3];
end