classdef KinectDataEval < KinectData

	methods
		function obj = KinectDataEval(skelData, headerDetails, calibrationDetails)
			obj@KinectData(skelData, headerDetails, calibrationDetails);
		end
		
		% New get Features
		[poseFeatures] = GetFeatures(obj)
	end
	methods (Access=protected) 
		peaks = findExcercisePeaks(obj)
	end
	
	
	
end
