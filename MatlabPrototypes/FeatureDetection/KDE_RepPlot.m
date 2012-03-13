function KDE_RepPlot(featureReps, exerciseType, cripComp_n)
	existingTypes={'squats', 'legExt', 'legRaise', 'armRaise'};
	if strcmp(exerciseType, 'squats')
		[tf,tc]=getTrainingData('squats', 1);
		exData=evalin('base', 'squat_KDE_params');
	elseif strcmp(exerciseType, 'armRaise')
		[tf,tc]=getTrainingData('armRaise', 1);
		exData=evalin('base', 'armRaise_KDE_params');
	elseif strcmp(exerciseType, 'legRaise')
		[tf,tc]=getTrainingData('legRaise', 1);
		exData=evalin('base', 'legRaise_KDE_params');
	elseif strcmp(exerciseType, 'legExt')
		[tf,tc]=getTrainingData('legExt', 1);
		exData=evalin('base', 'legExt_KDE_params');
	else
		err = MException('ResultChk:BadInput', ...
			'Excercise type %s is not valid, must be one of {%s}',...
			exerciseType, ...
			strtrim(sprintf('%s ', existingTypes{:})) ...
		);
		throw(err)
	end
	
	
	if cripComp_n==1
		kde_params=exData.critComp1;
	elseif cripComp_n==2
		kde_params=exData.critComp2;
	elseif cripComp_n==3
		kde_params=exData.critComp3;
	else
		err = MException('ResultChk:BadInput',...
			'Critical component must be one of [1,2,3]');
		throw(err)
	end
	
	disp(sprintf('Using features: [%i,%i]', kde_params.features(1:2)));
	
	
	
	contourFind2(tf,tc, cripComp_n,...
		kde_params.features, ...
		kde_params.limX, ...
		kde_params.limY, ...
		featureReps ...
	);
end