function [  ] = makePlots( goodData, badData1, badData2)
%UNTITLED7 Summary of this function goes here
%   Detailed explanation goes here

    subplot(2,2,1);
    plot(goodData(1,:), goodData(2,:),'go', ...
        badData1(1,:), badData1(2,:), 'ro', ...
        badData2(1,:), badData2(2,:), 'bo');
    xlabel({'Spine Stability'});
    ylabel({'Hip Angle'});
    
    subplot(2,2, 2);
    plot(goodData(1,:), goodData(3,:),'go', ...
        badData1(1,:), badData1(3,:), 'ro', ...
        badData2(1,:), badData2(3,:), 'bo');
    xlabel({'Spine Stability'});
    ylabel({'Knee Angle'});
    
    subplot(2,2, 3);
    plot(goodData(2,:), goodData(3,:),'go', ...
        badData1(2,:), badData1(3,:), 'ro', ...
        badData2(2,:), badData2(3,:), 'bo');
    xlabel({'Hip Angle'});
    ylabel({'Knee Angle'});

end

