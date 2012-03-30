function contourFind2(features, classes, critComp, useFeatures, wpX, wpY, plotFeatures)

mapPower=2;
%nContour=60;
nContour=60;
markSize=3;
xClip=0.05;
yClip=0.05;

plotDots=false;
% plotDots=true;


critCompScore=classes(:,critComp);
i2=find(critCompScore==2); i1=find(critCompScore==1); i0=find(critCompScore==0);

%% Graph KDEs of each cluster

[bandwidth2,density2,X2,Y2]=kde2d( ...
	[features(i2,useFeatures(1)),...
	features(i2,useFeatures(2))] ...
);
[bandwidth1,density1,X1,Y1]=kde2d( ...
	[features(i1,useFeatures(1)),... 
	features(i1,useFeatures(2))] ...
);
[bandwidth0,density0,X0,Y0]=kde2d( ...
	[features(i0,useFeatures(1)),...
	features(i0,useFeatures(2))] ...
);



xx=[min(min([X2,X1,X0])), max(max([X2,X1,X0]))];
yy=[min(min([Y2,Y1,Y0])), max(max([Y2,Y1,Y0]))];
if ~any(isnan(wpX))
	xx=wpX;
end
if ~any(isnan(wpY))
	yy=wpY;
end
disp(sprintf('Using limX: [%.1f,%.1f]', xx));
disp(sprintf('Using limY: [%.1f,%.1f]', yy));

pointsXY=plotFeatures(:, [useFeatures(1), useFeatures(2)]);
points_ib=pointsXY;
points_ob=pointsXY;
for i=1:size(pointsXY,1)
	[points_ib(i,:), points_ob(i,:)]=PointRemap2(...
		pointsXY(i,:), xx, yy, xClip,yClip);
end


	function plotReps(ib, ob)
		% plot(ps(:,1),ps(:,2), 'mx', 'MarkerSize', 6, 'LineWidth', 2);
		for j=1:size(ib,1)
			text(ib(j,1),ib(j,2),sprintf('%i',j), 'Color', 'm', 'FontWeight', 'bold');			
			if any(ib(j,:)~=ob(j,:))
				arrow(ib(j,:), ob(j,:), ... 
					'TipAngle', 20, 'BaseAngle', 40,  'Length', 6);
			end
		end
		
	end



ha=tight_subplot(1, 3, 0.01, 0.01, 0.01);

axes(ha(1));
hold on;
contourf(X2,Y2,density2,nContour);
colormap (c_colourMap('green').^mapPower);
xlim(xx); ylim(yy);
shading flat;
if plotDots==true
	plot(features(i2,useFeatures(1)), features(i2,useFeatures(2)), 'g.',...
		features(i1,useFeatures(1)), features(i1,useFeatures(2)), 'b.',...
		features(i0,useFeatures(1)), features(i0,useFeatures(2)), 'r.', 'MarkerSize', markSize);
end
plotReps(points_ib,points_ob);
hold off;
freezeColors

axes(ha(2));
hold on;
contourf(X1,Y1,density1,nContour);
colormap (c_colourMap('blue').^mapPower);
xlim(xx); ylim(yy);
shading flat;
if plotDots==true
	plot(features(i2,useFeatures(1)), features(i2,useFeatures(2)), 'g.',...
		features(i1,useFeatures(1)), features(i1,useFeatures(2)), 'b.',...
		features(i0,useFeatures(1)), features(i0,useFeatures(2)), 'r.', 'MarkerSize', markSize);
end
plotReps(points_ib,points_ob);
hold off;
freezeColors

axes(ha(3));
hold on;
contourf(X0,Y0,density0,nContour);
colormap (c_colourMap('red').^mapPower);
xlim(xx); ylim(yy);
shading flat;
if plotDots==true
	plot(features(i2,useFeatures(1)), features(i2,useFeatures(2)), 'g.',...
		features(i1,useFeatures(1)), features(i1,useFeatures(2)), 'b.',...
		features(i0,useFeatures(1)), features(i0,useFeatures(2)), 'r.', 'MarkerSize', markSize);
	
end
plotReps(points_ib,points_ob);
hold off;
freezeColors

set(ha,'XTickLabel',''); set(ha,'YTickLabel','');
set(ha,'XTick',[]); set(ha,'YTick',[]);

end
