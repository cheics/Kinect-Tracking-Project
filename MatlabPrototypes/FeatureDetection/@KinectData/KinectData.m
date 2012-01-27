classdef KinectData < handle
	properties
		% jnts => sorta enum for simplicity of feature methods
		jnts = struct(...
			'HIP_C', 1, 'SPINE', 2, 'SHOULDER_C', 3, 'HEAD', 4,...
			'SHOULDER_L', 5, 'ELBOW_L', 6, 'WRIST_L', 7, 'HAND_L', 8,...
			'SHOULDER_R', 9, 'ELBOW_R', 10, 'WRIST_R', 11, 'HAND_R', 12,...
			'HIP_L', 13, 'KNEE_L', 14, 'ANKLE_L', 15, 'FOOT_L', 16,...
			'HIP_R', 17, 'KNEE_R', 18, 'ANKLE_R', 19, 'FOOT_R', 20 ...
		);
		XYZ_IDS = struct('X', 1, 'Y', 2, 'Z', 3);
		
		% parameters in order to fuzzify joints for clustering
		dpw = 0;
		np = 1;
		
		% repitions of the excercise
		reps = 5;

		% joint to look at for peak detection
		peakDetectJoint
		joint_xyz
		
		% kinect skeleton data
		skelData
		% kinect data attributes
		dateHeader
		kinectHeight
		kinectTilt
		groundPlaneVector
		% derived features
		peakLocations
		featureVector
	end
	
	methods
		function obj = KinectData(skelData, headerDetails, calibrationDetails)
			% Get the skeleton data
			timeSteps=size(skelData); timeSteps=timeSteps(1);
			obj.skelData=reshape(skelData, [timeSteps 3 20]);
			
			% Default peak detection joint
			obj.peakDetectJoint=obj.jnts.KNEE_R;
			obj.joint_xyz=obj.XYZ_IDS.Y; %2
			
			% Get the file attributes
			obj.dateHeader=dataFileIn.dateHeader;
			obj.kinectHeight=dataFileIn.groundPlaneData.height;
			obj.kinectTilt=dataFileIn.groundPlaneData.kinectTilt;
			obj.groundPlaneVector=dataFileIn.groundPlaneData.gpVector;
		end
		
		
		function dataOutrestructureSkeletonData(timeStepDataIn)
			
		end
			
		function features = CalcFeatures(obj)
			features=[f_hipAngle() 0 0 0];
			obj.featureVector=features;
		end
		
		function peaks = FindPeaks(obj)
			peaks=obj.poseFinder(obj.peakDetectJoint, obj.joint_xyz, obj.reps, obj.dpw, obj.np);
			obj.peakLocations=peaks;
		end
		
		
		
	% Have private functions that calculate features (individually(
	% Have public functions that calculate
	%	Peaks
	%	All features/subset of features
	end
   
 	methods (Access = private)
		% Private utilities
		peakLocations = poseFinder(obj, joint_xyz, xyz, reps, dpw, np)
		
		% Features
		angleHip = f_hipAngle(obj);
 	end
end
