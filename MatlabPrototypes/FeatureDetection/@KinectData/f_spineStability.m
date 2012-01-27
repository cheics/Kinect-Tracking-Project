function spineScore  = f_spineStability(obj, frameNumber)
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here

spine=vertcat(...
	obj.getJointData(frameNumber, 'HEAD'), ...
	obj.getJointData(frameNumber, 'SHOULDER_C'), ...
	obj.getJointData(frameNumber, 'SPINE'), ...
	obj.getJointData(frameNumber, 'HIP_C') ...
);

[endpts2, spineResid]=obj.bestFitLine3d(spine);

v1=obj.groundPlane.dir;
v2=endpts2(2, :)-endpts2(1, :);

spineScore=180-acos(dot(v1, v2)./(norm(v1)*norm(v2)))*(180/pi);

end


