function DebugPeaks(obj, varargin)
	frameNumber = 0; 
	if numel(varargin) > 0
		frameNumber = varargin{1};
	end
	
	hold on;
	numberPoints=length(obj.peakDebug.jointData);
	plot(1:numberPoints, obj.peakDebug.jointData, ...
			1:numberPoints, obj.peakDebug.jointDataSmooth, ...
			obj.peakDebug.peakLocations, obj.peakDebug.jointDataSmooth(obj.peakDebug.peakLocations), 'ro' ...
		);
	plot(obj.peakDebug.peakLocations, obj.peakDebug.jointData(obj.peakDebug.peakLocations), ...
		'rs', 'MarkerSize', 10, 'LineWidth', 1.5);
	line_h=line([frameNumber,frameNumber], get(gca, 'YLim')); % indicator
	set(line_h, 'LineWidth', 2);
	set(line_h, 'LineStyle', ':');
	set(line_h, 'Color', 'k');
	plot(frameNumber, obj.peakDebug.jointData(frameNumber), ...
		'xk', 'MarkerSize', 15, 'LineWidth', 2);
	hold off;
	
end