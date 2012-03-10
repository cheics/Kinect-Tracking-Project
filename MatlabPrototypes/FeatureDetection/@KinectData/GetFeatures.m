function [poseFeatures, poseClass] = GetFeatures(obj)
	% Find some peaks
	obj.findExcercisePeaks();

	numberPoses=min(length(obj.peakLocations), length(obj.poseEval));
	%allocate outdata
	poseFeatures=zeros(numberPoses, 19);
	poseClass=obj.poseEval(1:numberPoses, :);

	% Find features at each frame
	for i = 1:numberPoses
		findPeak=obj.peakLocations(i);
		poseFeatures(i,:)=obj.poseFeatures(findPeak);
	end
	obj.featureResults.featureVector=poseFeatures;
	obj.featureResults.classVector=poseClass;
end