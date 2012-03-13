function [poseFeatures] = GetFeatures(obj)
	% Find some peaks
	obj.findExcercisePeaks();
	numberPoses=length(obj.peakLocations);
	%allocate outdata
	poseFeatures=zeros(numberPoses, 19);
	
	% Find features at each frame
	for i = 1:length(obj.peakLocations)
		findPeak=obj.peakLocations(i);
		poseFeatures(i,:)=obj.poseFeatures(findPeak);
	end
	obj.featureResults.featureVector=poseFeatures;
	disp('DOING IT LIVE');
end