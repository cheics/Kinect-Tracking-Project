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