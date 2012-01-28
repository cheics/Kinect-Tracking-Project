function headtilt = f_headtilt(obj, frameNumber)
	HeadVector=obj.getJointData(frameNumber, 'HEAD') ...
		-obj.getJointData(frameNumber, 'SHOULDER_C');
	ShoulderVector=obj.getJointData(frameNumber, 'SHOULDER_C') ...
		-obj.getJointData(frameNumber, 'SHOULDER_R');
    
    headtilt =180-acos(...
        dot(HeadVector, ShoulderVector)./(...
        norm(HeadVector)*norm(ShoulderVector)...
        ))*(180/pi);
end