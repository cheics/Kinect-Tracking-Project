function [kdObj] = FactoryKinectData(matFileName)
	kd=load(matFileName); kd=kd.kinectData; % load from the mat file
	
	% Extract the header data
	dataDetails=struct();
	dataDetails.date=kd.dateHeader;
	dataDetails.poseEval=kd.criticalComps';
	
	% Extract the calibration data
	calibData=struct();
	calibData.camOrientation=struct();
	calibData.camOrientation.tilt=kd.groundPlaneData.kinectTilt(1);
	calibData.camOrientation.height=kd.groundPlaneData.height;
	calibData.camOrientation.gpSkel=kd.groundPlaneData.gpVector(:, 1)';
	
	% Add some fake calibration details
	calibData.postureCorrection=struct();
	calibData.postureCorrection.shoulderTilt=0;
	calibData.postureCorrection.hipTilt=0;
	
	% Return an object
	%KinectData(skelData, headerDetails, calibrationDetails)
	kdObj=KinectData(kd.skelData', dataDetails, calibData);

end