function [  ] = makeClassPlots(A,score)
%UNTITLED7 Summary of this function goes here
%   Detailed explanation goes here
    Good = find (score == 3);
    BadB = find (score == 2);
    BadN = find (score == 1);
    
    subplot(2,2,1);
    plot(A(Good,1),A(Good,2),'go', ...
        A(BadB,1),A(BadB,2), 'ro', ...
        A(BadN,1),A(BadN,2), 'bo');

    xlabel({'Spine Stability'});
    ylabel({'Hip Angle'});
    
    subplot(2,2, 2);
    plot(A(Good,1),A(Good,3),'go', ...
        A(BadB,1),A(BadB,3), 'ro', ...
        A(BadN,1),A(BadN,3), 'bo');
    xlabel({'Spine Stability'});
    ylabel({'Knee Angle'});
    
    subplot(2,2, 3);
    plot(A(Good,2),A(Good,3),'go', ...
        A(BadB,2),A(BadB,3), 'ro', ...
        A(BadN,2),A(BadN,3), 'bo');
    xlabel({'Hip Angle'});
    ylabel({'Knee Angle'});

end