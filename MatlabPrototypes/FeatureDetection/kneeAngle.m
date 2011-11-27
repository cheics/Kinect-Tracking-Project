function [ angleKnee ] = kneeAngle( allDataVector )

    [hipLeft, hipCentre, hipRight]=deal(13,1, 17);
    [kneeRight, ankleRight]=deal(18, 19);
    hips=vertcat(...
        allDataVector(hipLeft, :), ...
        allDataVector(hipCentre, :), ...
        allDataVector(hipRight, :)...
    );
    
    shinVector=allDataVector(kneeRight, :)-allDataVector(ankleRight, :);
    thighVector=allDataVector(hipRight, :)-allDataVector(kneeRight, :);
    
    angleKnee=180-acos(...
        dot(shinVector, thighVector)./(...
        norm(shinVector)*norm(thighVector)...
        ))*(180/pi);
    
%     debugPose(allDataVector);
%     hold on
%     plot3(endpts1(:, 1), endpts1(:, 3), endpts1(:, 2), 'b-x');
%     axis equal; 
%     pause

end


