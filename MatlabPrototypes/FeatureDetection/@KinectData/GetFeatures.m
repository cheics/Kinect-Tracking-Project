function [poseFeatures, classFeatures] = GetFeatures(obj)
	% Find some peaks
	findPeaks();

	numberPoses=length(obj.peakLocations);
	%allocate outdata
	poseFeatures=zeros(numberPoses, 15);
	classFeatures=obj.poseEval(1:numberPoses, :);

	% Find features at each frame
	for i = 1:length(obj.peakLocations)
		findPeak=obj.peakLocations(i);
		poseFeatures(i,:)=obj.poseFeatures(findPeak);
	end
end