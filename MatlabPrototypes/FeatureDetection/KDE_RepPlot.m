function KDE_RepPlot(featureReps, exerciseType, varargin)
	existingTypes={'squats', 'legExt', 'legRaise', 'armRaise'};
	if strcmp(exerciseType, 'squats')
		tf=evalin('base', 'tf_squats'); 
		tc=evalin('base', 'tc_squats'); 
		exData=evalin('base', 'squat_KDE_params');
	elseif strcmp(exerciseType, 'armRaise')
		tf=evalin('base', 'tf_armRaise'); 
		tc=evalin('base', 'tc_armRaise'); 
		exData=evalin('base', 'armRaise_KDE_params');
	elseif strcmp(exerciseType, 'legRaise')
		tf=evalin('base', 'tf_legRaise'); 
		tc=evalin('base', 'tc_legRaise'); 
		exData=evalin('base', 'legRaise_KDE_params');
	elseif strcmp(exerciseType, 'legExt')
		tf=evalin('base', 'tf_legExt'); 
		tc=evalin('base', 'tc_legExt'); 
		exData=evalin('base', 'legExt_KDE_params');
	else
		err = MException('ResultChk:BadInput', ...
			'Excercise type %s is not valid, must be one of {%s}',...
			exerciseType, ...
			strtrim(sprintf('%s ', existingTypes{:})) ...
		);
		throw(err)
	end
	
	if numel(varargin) > 0
		cripComp_n = varargin{1};
	elseif  isfield(exData, 'cc_pref')
		cripComp_n=exData.cc_pref;
	else
		cripComp_n=1;
	end
	
	disp(sprintf('Using critical comp %i', cripComp_n));
	
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