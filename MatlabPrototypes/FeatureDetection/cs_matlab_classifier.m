function [outClass] = cs_matlab_classifier(exerciseType, kd, gp)
	
	existingTypes={'squats', 'legExt', 'legRaise', 'armRaise'};
	if strcmp(exerciseType, 'squats')
		[tf,tc]=getTrainingData('squats', 1);
	elseif strcmp(exerciseType, 'armRaise')
		[tf,tc]=getTrainingData('armRaise', 1);
	elseif strcmp(exerciseType, 'legRaise')
		[tf,tc]=getTrainingData('legRaise', 1);
	elseif strcmp(exerciseType, 'legExt')
		[tf,tc]=getTrainingData('legExt', 1);
	else
		err = MException('ResultChk:BadInput', ...
			'Excercise type %s is not valid, must be one of {%s}',...
			exerciseType, ...
			strtrim(sprintf('%s ', existingTypes{:})) ...
		);
		throw(err)
	end
	
	cc=MakeClassifier('BAYES', false);
	cc.trainClassifiers(tf, tc);
	
	kk=FactoryKinectData_CS(exerciseType, kd, gp);
	kk_features=kk.GetFeatures();
	outClass=zeros(10,3)-1;
	
	for i=1:size(kk_features,1)
		outClass(i,:)=cc.classify(kk_features(i,:));
	end
	outClass=reshape(outClass', 1, size(outClass,1)*size(outClass,2))';
	
end
	
	

	