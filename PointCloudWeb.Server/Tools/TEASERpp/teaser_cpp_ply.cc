// An example showing TEASER++ registration with the Stanford bunny model
#include <chrono>
#include <iostream>
#include <fstream>
#include <random>
#include <filesystem>

#include <Eigen/Core>

#include <teaser/ply_io.h>
#include <teaser/registration.h>

// Macro constants for generating noise and outliers
#define NOISE_BOUND 0.05
// #define N_OUTLIERS 1700
// #define OUTLIER_TRANSLATION_LB 5
// #define OUTLIER_TRANSLATION_UB 10

inline double getAngularError(Eigen::Matrix3d R_exp, Eigen::Matrix3d R_est)
{
  return std::abs(std::acos(fmin(fmax(((R_exp.transpose() * R_est).trace() - 1) / 2, -1.0), 1.0)));
}

int main(int argc, char **argv)
{
  std::cout << "You have entered " << argc
            << " arguments:"
            << "\n";

  for (int i = 0; i < argc; ++i)
    std::cout << argv[i] << "\n";

  auto src_fileName = argv[1];
  auto tgt_fileName = argv[2];

  // Load the .ply file
  teaser::PLYReader reader;
  teaser::PointCloud src_cloud;
  auto status = reader.read(src_fileName, src_cloud);
  int src_cloud_size = src_cloud.size();

  // Convert the point cloud to Eigen
  Eigen::Matrix<double, 3, Eigen::Dynamic> src(3, src_cloud_size);
  for (size_t i = 0; i < src_cloud_size; ++i)
  {
    src.col(i) << src_cloud[i].x, src_cloud[i].y, src_cloud[i].z;
  }

  teaser::PointCloud tgt_cloud;
  status = reader.read(tgt_fileName, tgt_cloud);
  int tgt_cloud_size = tgt_cloud.size();

  // Convert the point cloud to Eigen
  Eigen::Matrix<double, 3, Eigen::Dynamic> tgt(3, tgt_cloud_size);
  for (size_t i = 0; i < tgt_cloud_size; ++i)
  {
    tgt.col(i) << tgt_cloud[i].x, tgt_cloud[i].y, tgt_cloud[i].z;
  }

  // Run TEASER++ registration
  // Prepare solver parameters
  teaser::RobustRegistrationSolver::Params params;
  params.noise_bound = NOISE_BOUND;
  params.cbar2 = 1;
  params.estimate_scaling = false;
  params.rotation_max_iterations = 100;
  params.rotation_gnc_factor = 1.4;
  params.rotation_estimation_algorithm =
      teaser::RobustRegistrationSolver::ROTATION_ESTIMATION_ALGORITHM::GNC_TLS;
  params.rotation_cost_threshold = 0.005;

  // Solve with TEASER++
  teaser::RobustRegistrationSolver solver(params);
  std::chrono::steady_clock::time_point begin = std::chrono::steady_clock::now();
  solver.solve(src, tgt);
  std::chrono::steady_clock::time_point end = std::chrono::steady_clock::now();

  auto solution = solver.getSolution();

  // Compare results
  std::cout << "=====================================" << std::endl;
  std::cout << "          TEASER++ Results           " << std::endl;
  std::cout << "=====================================" << std::endl;
  std::cout << "Estimated rotation: " << std::endl;
  std::cout << solution.rotation << std::endl;
  std::cout << std::endl;
  std::cout << "Estimated translation: " << std::endl;
  std::cout << solution.translation << std::endl;
  std::cout << std::endl;
  std::cout << "Time taken (s): "
            << std::chrono::duration_cast<std::chrono::microseconds>(end - begin).count() /
                   1000000.0
            << std::endl;
}
