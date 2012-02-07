function DebugAll(obj)
	%for k = 1:length(obj.peakDebug.jointData)
	for k = 1:30
		obj.DebugPeaks(k);
		M(k) = getframe;
		clf;
	end
	movie(M,5,27)
end
