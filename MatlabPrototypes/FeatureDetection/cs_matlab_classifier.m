function [outClass] = cs_matlab_classifier(exerciseType, kd, gp)
	
	existingTypes={'squats', 'legExt', 'legRaise', 'armRaise'};
	if strcmp(exerciseType, 'squats')
		cc=evalin('base', 'cl_squats');
	elseif strcmp(exerciseType, 'armRaise')
		cc=evalin('base', 'cl_armRaise');
	elseif strcmp(exerciseType, 'legRaise')
		cc=evalin('base', 'cl_legRaise');
	elseif strcmp(exerciseType, 'legExt')
		cc=evalin('base', 'cl_legExt');
	else
		err = MException('ResultChk:BadInput', ...
			'Excercise type %s is not valid, must be one of {%s}',...
			exerciseType, ...
			strtrim(sprintf('%s ', existingTypes{:})) ...
		);
		throw(err)
	end
	
	kk=FactoryKinectData_CS(exerciseType, kd, gp);
	kk_features=kk.GetFeatures();
	outClass=zeros(10,3)-1;
	
	for i=1:size(kk_features,1)
		outClass(i,:)=cc.classify(kk_features(i,:));
	end
	
	for i=1:size(outClass,1)
		for j=1:size(outClass,2)
			if isnan(outClass(i,j))
				outClass(i,j)=-2;
			end
		end
				
	end
	%outClass=reshape(outClass', 1, size(outClass,1)*size(outClass,2))';
	KDE_RepPlot(kk_features, exerciseType)
	screen_size=get(0,'ScreenSize');
	set(gcf, 'Position', [0,0, screen_size(3), screen_size(4)]);
	csvwrite('results.csv', outClass)
	pause(10);
	
end
	
	

	