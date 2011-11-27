function [ scoresArray ] = GetFeatures( bigData, valid_ts)

    kneeRight_IDS=[52,53,54];
    hipRight_IDS=[49,50,51];
    hipCentre_IDS=[1,2,3];
    
    shoulderCentre_IDS=[7,8,9];
    shoulderLeft_IDS=[13,14,15];
    shoulderRight_IDS=[25,26,27];
    
    spine_IDS=[4,5,6];
    foot_IDS=[58,59,60];

    cc=reshape(bigData, size(bigData,1), 3, 20);
    poses=cc(valid_ts, :, :);
        
    scoresArray=zeros(3,size(poses,1));
    for i = 1:size(poses,1)
        specificPose=reshape(poses(i, :, :), 3, 20).';
        scoresArray(1,i)=spineStability(specificPose);
        scoresArray(2,i)=hipAngle(specificPose);
        scoresArray(3,i)=kneeAngle(specificPose);
    end
end

