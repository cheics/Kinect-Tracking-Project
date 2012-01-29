function peaks = findPeaks(obj)
%	FindPeaks	Wrapper function for poseFinder, includes several
%	default values as well as calibration data
	peaks=obj.poseFinder(obj.peakDetectJoint, obj.joint_xyz, obj.repsGuess, obj.dpw, obj.np);
	obj.peakLocations=peaks;
end