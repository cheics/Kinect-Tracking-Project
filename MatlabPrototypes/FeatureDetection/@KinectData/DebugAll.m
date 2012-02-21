function DebugAll(obj, varargin)
	showFeatures=true;
	frameRate=30;
	if numel(varargin) > 0
		showFeatures = varargin{1};
	end
	if numel(varargin) > 1
		numberFrames = varargin{2};
	else
		numberFrames=length(obj.peakDebug.jointData);
	end
	
	
	% Window stuff...
	scrsz = get(0,'ScreenSize');
	figure('Position',[7 50 scrsz(3)/2 scrsz(4)-(50+85)])
	
		
	for k = 1:numberFrames
		subplot(3,2,[1,3]);
		obj.DebugPose(k)
		if showFeatures==true
			subplot(3,2,[2,4]);
			barh(obj.featureResults.featureVector);
		end
		subplot(3,2,5:6);
		obj.DebugPeaks(k);
		M(k) =  getframe(gcf);
		clf;
	end
	axes('pos',[0 0 1 1],'visible','off'); 
	movie(M,20,frameRate)
end
