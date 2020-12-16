# InteriorDesign
* Package 1: [Link Package](https://drive.google.com/drive/folders/1Dremv0NSHaxxHPKwRbhe6HGWB2PStEKB?usp=sharing)
* Package 2: [Demo](https://www.youtube.com/watch?v=dY96xIEpKC8). Là repo này. Do đang cài đặt chức năng cho VR controller, hiện tại package này chưa sử dụng được.
# Hướng dẫn cài đặt package 1
1. Tạo một project [HDRP](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@6.7/manual/Getting-started-with-HDRP.html) mới
2. Xóa thư mục Asset
3. Import file unitypackage.

# Hướng dẫn sử dụng package 1
* Chon các scene trong folder `Scenes`
  * Trong scene `Room01_light` có demo 2 kiểu thiết kế bằng đối tượng `----Env----\Design*`, toggle active để thay đổi.
  * Scene `Room01_light` và `Room01_night` demo 2 môi trường ánh sáng khác nhau
  
 
* Đặt các bức tranh:  
  * Yêu cầu ở chế độ editor, và icon [gizmos](https://lh3.googleusercontent.com/proxy/y0cLst_y58xk96TxPRgbw7eCdabF719tLhFD9GvMa2dSzX8BBScegpNrAE_wLV3eLPJkJaCAUm1s_DbDIZFXaSUZPsM_OFHFtpEwoTrCcCkpvJhWSMxI) được bật.
  * Set các đối tượng bức tường đặt được tranh với tag `Wall`
  * Thêm script `Picture` (trong folder `Scripts`) cho các đối tượng là bức tranh.
  * Trong chế độ Editor của Unity, chọn đối tượng có script `Picture`, sau đó trỏ chuột đến vị trí muốn đặt tranh, nhấn `B`.
  * Ctrl + Z để undo

* Đổi nội dung tranh:  
(Todo: Kéo thả image vào đối tượng `Picture`)
  * Kéo ảnh trong thư mục image vào `base map` của `material` của `Picture`

# Hướng dẫn sử dụng package 2
Sử dụng trong chế độ Unity play
* Có 2 chế độ:
  * Editmode: Loại bỏ hiệu ứng của rigitBody và thêm các chức năng: di chuyển, xoay vật thể (chưa cài đặt scale)
  * ShowMode: Kích hoạt các hiệu ứng vật lý, các vật sẽ rơi xuống
Chuyển đổi giữa 2 chế độ bằng cách nhấn `E`  

* Có 2 chế độ Camera chuyển đổi bằng cách nhấn `F`  
  * FPS: Nhận vật di chuyển xung quanh căn phòng  
  * TPS: Tương tự với camera của Unity Editor

* Selection:
  * Thêm script `Selectable` cho các object có thể lựa chọn được
  * Có thể lựa chọn 1 hoặc 1 nhóm các vật để tương tác

* Chức năng trong Editmode:  
  * Transform: Kích hoạt bằng phím `T`:  
    * `G` để chọn chức năng di chuyển  
    * `R` để chọn chức năng Rotation   
Sau khi chọn chức năng, nhấn giữ các phím x, y, z để chọn trục (Có thể nhấn 2 phím để di chuyển trên mặt phẳng), sau đó di chuyển chuột.
