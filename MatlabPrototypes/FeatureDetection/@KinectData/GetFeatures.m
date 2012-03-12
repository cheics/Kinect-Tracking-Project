function [poseFeatures, poseClass] = GetFeatures(obj)
	% Find some peaks
	obj.findExcercisePeaks();

	numberPoses=min(length(obj.peakLocations), length(obj.poseEval));
	
	if obj.dpw ~= 0
		newEval=zeros(length(obj.peakLocations), 3);
		for i = 1:length(obj.poseEval)
			for j = 1:round((obj.dpw*2+1)*obj.np)
				newEval(3*(i-1)+j, :)=obj.poseEval(i,:);
			end
		end
		obj.poseEval=newEval;
		numberPoses=length(obj.poseEval);
	end
	
	
	%allocate outdata
	poseFeatures=zeros(numberPoses, 19);
	poseClass=obj.poseEval(1:numberPoses, :);
	
	% Find features at each frame
	for i = 1:length(obj.peakLocations)
		findPeak=obj.peakLocations(i);
		if obj.dpw ~= 0
			findPeak=abs(findPeak);
			if findPeak==0
				findPeak=1;
			elseif findPeak > size(obj.skelData,1)
				findPeak=size(obj.skelData,1);
			end
		end
		poseFeatures(i,:)=obj.poseFeatures(findPeak);
	end
	obj.featureResults.featureVector=poseFeatures;
	obj.featureResults.classVector=poseClass;
end