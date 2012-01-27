function angleHip = f_hipAngle(obj, frameNumber)
	hips=vertcat(...
		obj.getJointData(frameNumber, 'HIP_L'), ...
		obj.getJointData(frameNumber, 'HIP_C'), ...
		obj.getJointData(frameNumber, 'HIP_R') ...
    );
    
    endpts1=obj.bestFitLine3d(hips);
    hipVector=endpts1(1, :)-endpts1(2, :);
    hipVector(:, 2) = 0; %% reduce y dimensionality
    
    kneeVector=obj.getJointData(frameNumber, 'HIP_R')-obj.getJointData(frameNumber, 'KNEE_R');
    kneeVector(:, 2) = 0; %% reduce y dimensionality
    
    angleHip=180-acos(...
        dot(hipVector, kneeVector)./(...
        norm(hipVector)*norm(kneeVector)...
        ))*(180/pi);
    
%     debugPose(allDataVector);
%     hold on
%     line1=plot3(endpts1(:, 1), endpts1(:, 3), endpts1(:, 2), 'k-x');
%     kv=vertcat(allDataVector(hipRight, :),allDataVector(kneeRight, :));
%     line2=plot3(kv(:, 1), kv(:, 3), kv(:, 2), 'k-x');
%     set(line1,'LineWidth',3);
%     set(line2,'LineWidth',3);
%     axis equal; 
%     pause

end


