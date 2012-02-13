function DebugPoseShowFeature(obj, frameNumber, highlightVector)
	% graph axes...
    gx=1;
    gy=3;
    gz=2;
	
	thickLine=zeros(0,3);
	for i=1:length(highlightVector)
		thickLine=vertcat(thickLine, ...
			obj.getJointData(frameNumber, highlightVector{i}) ...
		);
	end
	
	
	obj.DebugPose(frameNumber);
	hold on;
	thickLine
	plot3(...
		thickLine(:, gx), thickLine(:, gy), thickLine(:, gz), 'k-', 'LineWidth', 4 ...
	);
	hold off;
			
end