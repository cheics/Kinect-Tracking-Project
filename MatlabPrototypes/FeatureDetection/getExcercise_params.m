function [dataFilePaths,peakDetectJoint,peakDetectDim,detectPeakHigh]= getExcercise_params(exType)
	if strcmp(exType, 'squats')
		dataFilePaths={'Squats1', 'Squats2'};
		peakDetectJoint='HIP_C';
		peakDetectDim='Y';
		detectPeakHigh=false;
	elseif strcmp(exType, 'legExt')
		dataFilePaths={'LegExt1'};
		peakDetectJoint='ANKLE_R';
		peakDetectDim='X';
		detectPeakHigh=true;
	elseif strcmp(exType, 'legRaise')
		dataFilePaths={'LegRaise1'};
		peakDetectJoint='ANKLE_L';
		peakDetectDim='Y';
		detectPeakHigh=true;
	elseif strcmp(exType, 'armRaise')
		dataFilePaths={'ArmRaise1'};
		peakDetectJoint='HAND_L';
		peakDetectDim='Y';
		detectPeakHigh=true;
	else
		err = MException('ResultChk:BadInput', ...
			'Excercise type %s is not valid, must be one of {%s}',...
			dataFile, ...
			strtrim(sprintf('%s ', existingTypes{:})) ...
		);
		throw(err)
	end
end