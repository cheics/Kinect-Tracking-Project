function [ xx ] = debugPose( poseData )
%debugPose.m Graphs a pose
%   poseData:     20x3 array of joint data
    
    fp=poseData';
    % graph axes...
    gx=1;
    gy=3;
    gz=2;
    
    head_spine_hips=[4,3,2,1];
    hand_to_hand=[8,7, 6, 5, 3, 9, 10, 11, 12];
    foot_to_foot=[16,15,14,13, 1, 17, 18, 19, 20];
    
    
    plot3(...
        fp(gx,head_spine_hips), fp(gy,head_spine_hips), fp(gz,head_spine_hips), '-s',...
        fp(gx,hand_to_hand), fp(gy,hand_to_hand), fp(gz,hand_to_hand), '-s',...
        fp(gx,foot_to_foot), fp(gy,foot_to_foot), fp(gz,foot_to_foot), '-s'...
    );
    axis equal;
end

