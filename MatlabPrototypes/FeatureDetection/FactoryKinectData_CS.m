function [kdObj] = FactoryKinectData_CS(kinectData_cs, gp)

	% Extract the header data
	dataDetails=struct();
	dataDetails.date=NaN;
	dataDetails.poseEval=NaN;
	
	% Extract the calibration data
	calibData=struct();
	calibData.camOrientation=struct();
	calibData.camOrientation.tilt=NaN;
	calibData.camOrientation.height=NaN;
	calibData.camOrientation.gpSkel=gp';
	
	% Add some fake calibration details
	calibData.postureCorrection=struct();
	calibData.postureCorrection.shoulderTilt=0;
	calibData.postureCorrection.hipTilt=0;
	
	% Return an object
	%KinectData(skelData, headerDetails, calibrationDetails)
	kdObj=KinectData(kinectData_cs, dataDetails, calibData);

end