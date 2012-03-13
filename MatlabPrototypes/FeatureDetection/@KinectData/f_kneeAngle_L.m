function angleKnee = f_kneeAngle_L(obj, frameNumber)
	shinVector=obj.getJointData(frameNumber, 'KNEE_L') ...
		-obj.getJointData(frameNumber, 'ANKLE_L');
	thighVector=obj.getJointData(frameNumber, 'HIP_L') ...
		-obj.getJointData(frameNumber, 'KNEE_L');
    
    angleKnee=180-acos(...
        dot(shinVector, thighVector)./(...
        norm(shinVector)*norm(thighVector)...
        ))*(180/pi);
end

