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
        % kinect calibration data
		calibData
        
		% derived features
		peakLocations
		featureVector
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
			obj.peakDetectJoint='KNEE_R'; %obj.jnts.KNEE_R;
			obj.joint_xyz='Y'; %obj.XYZ_IDS.Y;

			% Get the file attributes
			obj.dateHeader=headerDetails;
			obj.calibData=calibrationDetails;
			
			% Ground plane
			% Hardcoded for debug
			obj.groundPlane.loc=[0.0664, -0.6309, 2.7400];
			obj.groundPlane.dir=[0, 1, 0];
		end
					
		function peaks = FindPeaks(obj)
		%	FindPeaks	Wrapper function for poseFinder, includes several
		%	default values as well as calibration data
			peaks=obj.poseFinder(obj.peakDetectJoint, obj.joint_xyz, obj.reps, obj.dpw, obj.np);
			obj.peakLocations=peaks;
		end
		
		% Other utils
        function features = poseFeatures(obj, frameNumber)
			features=[...
				f_hipAngle(obj, frameNumber),...
				f_kneeAngle(obj, frameNumber), ...
				f_spineStability(obj, frameNumber) ...
			];
			obj.featureVector=features;
        end
	end
	

    
    % Private utilities
 	methods (Access = public)
		% Prototypes
        peakLocations = poseFinder(obj, joint_xyz, xyz, reps, dpw, np)
		debugPose(obj, frameNumber)
      
        
        function jointXYZ = getJointData(obj, frameNumber, jointName)
            jointXYZ=obj.skelData(frameNumber, :, obj.jnts.(jointName));
        end
    end
        
    % Feature prototypes are private functions
    methods (Access = private)
		angleHip = f_hipAngle(obj, frameNumber)
		angleKnee = f_kneeAngle(obj, frameNumber)
		spineScore = f_spineStability(obj, frameNumber)
	end
	
	methods (Access = private, Static=true)
		[endpts, residuals] = bestFitLine3d( X )
	end
	
end
