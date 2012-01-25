function angleHip = f_hipAngle(poseNumber)
	
	hips=[obj.skelData[obj.jnts
    [hipLeft, hipCentre, hipRight]=deal(13,1, 17);
    [kneeRight]=deal(18);
    hips=vertcat(...
        allDataVector(hipLeft, :), ...
        allDataVector(hipCentre, :), ...
        allDataVector(hipRight, :)...
    );
    
    endpts1=Utils.bestFitLine3d(hips);
    hipVector=endpts1(1, :)-endpts1(2, :);
    hipVector(:, 2) = 0; %% reduce y dimensionality
    
    kneeVector=allDataVector(hipRight, :)-allDataVector(kneeRight, :);
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


