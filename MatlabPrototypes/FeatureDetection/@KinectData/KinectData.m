classdef KinectData < handle
%	properties (Access = protected, Hidden = true)
	properties (Access = public, Hidden = false)
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

		% joint to look at for peak detection
		peakDetectJoint
		joint_xyz
		repsGuess
		
		% kinect skeleton data
		skelData
		% kinect calibration data
		calibData
		
		% peakDebugData
		peakDebug
		% debugDimensions
		debugDimensions
	end
	
	properties (Access = public, Hidden = false)
		% kinect data attributes
		dateHeader
        % training data class
		poseEval
	      
		% derived features
		peakLocations
		featureResults
		groundPlane
	end
	
	% Public simple methods
	methods
		function obj = KinectData(skelData, headerDetails, calibrationDetails)
		%	KinectData	KinectData default constructor
		%
		%	Synopsis:
		%		obj = KinectData(skelData, headerDetails, calibrationDetails)
		%   
		%	Input:
		%		skelData			= A (nx60) matrix of KinectSkeleton data
		%		headerDetails		= A struct that contains some details about
		%								the collected Kinect data
		%		calibrationDetails	= A struct containing information about the
		%								Kinect camera orientation, the ground plane
		%								and the posture correction
		%
		%	Output:
		%		obj = the KinectData object
			timeSteps=size(skelData); timeSteps=timeSteps(1);
			obj.skelData=reshape(skelData, [timeSteps 3 20]);

			% Default peak detection joint
			obj.peakDetectJoint='HIP_C'; %obj.jnts.KNEE_R;
			obj.joint_xyz='Y'; %obj.XYZ_IDS.Y;

			% Get the file attributes
			obj.dateHeader=headerDetails.date;
			obj.poseEval=headerDetails.poseEval;
			obj.repsGuess=size(headerDetails.poseEval); obj.repsGuess=obj.repsGuess(1);
			obj.calibData=calibrationDetails;
			
			% Method 1 uses skel data
			% Method 2 uses trig
			obj.calibrateCamera(1);
			
			
			obj.featureResults=struct();
			obj.peakDebug=struct();
			obj.debugDimensions=struct();
		end
		
		% Public Utilities
		[poseFeatures, classFeatures] = GetFeatures(obj)
		DebugPose(obj, frameNumber)
		DebugPeaks(obj, varargin)
		DebugAll(obj, varargin)
	end
	

    
    % Private utilities
 	methods (Access = private)
        %	poseFinder		Finds the likely maximums for data
		[peakLocations, rawDataIn, lpf_dataIn] = poseFinder(obj, joint_xyz, xyz, reps, dpw, np, findMin)
		%	poseFeatures	Gets Feature Vector for a frame
		features = poseFeatures(obj, frameNumber)
		%	findExcercisePeaks		Wrapper function for poseFinder, includes several
		peaks = findExcercisePeaks(obj)
		%	getJointData	Get the XYZ data for a joint by name
		jointXYZ = getJointData(obj, frameNumber, jointName)
		%	calibrateCamera	Calibrates the ground plane
		calibrateCamera(obj,methodNumber)
    end
        
    % Feature prototypes are private functions
    methods (Access = private)
		angleHip = f_hipAngle(obj, frameNumber)
		angleKnee = f_kneeAngle(obj, frameNumber)
		spineScore = f_spineStability(obj, frameNumber)
		
		elbowshoulder_L = f_elbowAngle_L(obj, frameNumber)
		elbowshoulder_R = f_elbowAngle_R(obj, frameNumber)
		
		headtilt = f_headtilt(obj, frameNumber)
		headpitch = f_headpitch(obj, frameNumber)
		
		anklelevel = f_ankleLevel(obj, frameNumber)
		handlevel = f_handLevel(obj, frameNumber)
		hiplevel = f_hipLevel(obj, frameNumber)
		shoulderlevel = f_shoulderLevel(obj, frameNumber)
	end
	
	methods (Access = private, Static=true)
		[endpts, residuals] = bestFitLine3d( X )
	end
	
end
