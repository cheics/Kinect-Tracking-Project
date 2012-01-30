function [poseFeatures, poseClass] = GetFeatures(obj)
	% Find some peaks
	obj.findExcercisePeaks();

	numberPoses=length(obj.peakLocations);
	%allocate outdata
	poseFeatures=zeros(numberPoses, 15);
	poseClass=obj.poseEval(1:numberPoses, :);

	% Find features at each frame
	for i = 1:length(obj.peakLocations)
		findPeak=obj.peakLocations(i);
		poseFeatures(i,:)=obj.poseFeatures(findPeak);
	end
	obj.featureResults.featureVector=poseFeatures;
	obj.featureResults.classVector=poseClass;
end