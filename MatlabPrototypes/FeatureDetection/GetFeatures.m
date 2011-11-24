function [ scoresArray ] = GetFeatures( bigData, valid_ts)

    kneeRight_IDS=[52,53,54];
    hipRight_IDS=[43,44,45];
    hipCentre_IDS=[1,2,3];
    spine_IDS=[4,5,6];
    foot_IDS=[58,59,60];

    ss=zeros(3,size(valid_ts,2));

    for i = 1:size(valid_ts,2)
        cc1=bigData(valid_ts(1,i), horzcat(kneeRight_IDS,hipCentre_IDS,spine_IDS));
        cc2=bigData(valid_ts(1,i), horzcat(hipCentre_IDS,hipRight_IDS,kneeRight_IDS));
        cc3=bigData(valid_ts(1,i), horzcat(hipRight_IDS,kneeRight_IDS, foot_IDS));

        ss(1,i)=spineStability(cc1(:,1:3), cc1(:,4:6), cc1(:,7:9));
        ss(2,i)=hipAngle(cc2(:,1:3), cc2(:,4:6), cc2(:,7:9));
        ss(3,i)=kneeAngle(cc3(:,1:3), cc3(:,4:6), cc3(:,7:9));
    end

    scoresArray=ss;
end

