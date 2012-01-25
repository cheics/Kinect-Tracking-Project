function [ angleHip ] = hipAngle( allDataVector )

    [hipLeft, hipCentre, hipRight]=deal(13,1, 17);
    [kneeRight]=deal(18);
    hips=vertcat(...
        allDataVector(hipLeft, :), ...
        allDataVector(hipCentre, :), ...
        allDataVector(hipRight, :)...
    );
    
    endpts1=bestFitLine3d(hips);
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
%     plot3(endpts1(:, 1), endpts1(:, 3), endpts1(:, 2), 'b-x');
%     axis equal; 
%     pause

end


