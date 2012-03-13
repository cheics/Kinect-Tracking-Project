function peaks = findExcercisePeaks(obj)
%	FindPeaks	Wrapper function for poseFinder, includes several
%	default values as well as calibration data
	[peaks, rawDataIn, lpf_dataIn]=obj.poseFinder(obj.peakDetectJoint, obj.joint_xyz, 5, obj.dpw, obj.np, obj.findMax);
	obj.peakDebug.peakLocations=peaks;
	obj.peakDebug.jointData=rawDataIn;
	obj.peakDebug.jointDataSmooth=lpf_dataIn;
	obj.peakLocations=peaks;
end